<script lang="ts">
  export default {
    name: 'DatePicker'
  }
</script>

<script setup lang=ts>

const { label, icon, mask, name } = defineProps<{
    name:string
    label:string
    icon?:string | undefined
    mask?: string | undefined
}>()

/**
 * Create a default value ref
 */
const value = $ref('')

/**
 * Define an emit for updating the v-model in the parent
 */
const emit = defineEmits(['update:modelValue'])

//! Remove later
/**
 * Create a function to take in the new value and emit it
 * 
 * @param value new model value
 */
function emitInput(value: number | Date | string): void {
    emit('update:modelValue', value)
}

const required = (value: any): string | boolean => {
    return (value === '' || value === null || value.length < 1) ? '' : true
}
</script>

<template>
    <div class="q-px-xs">
        <div class="row">
            <div class="col">
                <q-input
                    :ref="name"
                    v-model="value"
                    type="date"
                    square
                    filled
                    stack-label
                    value=""
                    :label="label"
                    :rules="[required]"
                    :mask="mask"
                    fill-mask
                    unmasked-value
                    clearable
                    @update:model-value="(value:any) => { emitInput(value) }"
                >
                    <template #after >
                        <tool-tip :icon="icon" text="hello"/>
                    </template>
                </q-input>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>