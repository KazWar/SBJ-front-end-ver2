<script lang="ts">
  export default {
    name: 'FileUploader'
  }
</script>

<script setup lang=ts>
import ToggleButton from './components/toggle-button.vue'
import UploadPreviewCard from './components/upload-preview-card.vue'
import { Base64Encode } from '@/common'
import { Ref } from 'vue'

const { label, icon } = defineProps<{
    label:string
    icon?:string | undefined
}>()

/**
 * Create default refs
 */
const file:any = $ref(null)
let fileUrl:string = $ref('')
let toggle = $ref(false)

/**
 * Define an emit for updating the v-model in the parent
 */
const emit = defineEmits(['update:modelValue'])

/**
 * Emit the field input to the parent 
 * @param value File
 */
const emitInput = async (value: File): Promise<void> => {
    if (value){
        fileUrl = filePath(value)
        toggle = true
    } else {
        fileUrl = ''
        toggle = false
    }

    emit('update:modelValue', value ? [await Base64Encode(value)]: [value])
}

/**
 * 
 */
const filePath = (file:File):string => {
    return URL.createObjectURL(file)
}

const required = (ref: Ref): string | boolean => {
    return (ref === null) ? '' : true
}

</script>

<template>
    <div class="q-px-xs">
        <div class="row">
            <div class="col">
                <q-file
                    v-model="file"
                    clearable
                    :label="label"
                    filled
                    stack-label
                    square
                    accept=".jpg, .png, .pdf"
                    :rules="[required]"
                    :lazy-rules=true
                    @update:model-value="(value) => { emitInput(value) }"
                >
                    <template #prepend>
                        <q-icon name="attach_file" />
                    </template>

                    <template #append>
                        <toggle-button 
                            :toggle="toggle" 
                            :file-url="fileUrl" 
                            @toggled="value => { toggle = value }"
                        />
                    </template>
                </q-file>
            </div>
        </div>
        <div v-if="toggle" class="q-pb-md"  >
            <div class="row justify-center toggle-background ">
                <upload-preview-card :file-url="fileUrl"/>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.toggle-background{
    background-color: rgb(223, 223, 223) !important;
}

</style>