<script lang="ts">
  export default {
    name: 'InputText'
  }
</script>

<script setup lang=ts>

const { label, icon, mask, type, name } = defineProps<{
    label:string
    name:string
    icon?:string | undefined
    mask?: string | undefined
    type?: 
        | "text"
        | "password"
        | "textarea"
        | "email"
        | "search"
        | "tel"
        | "number"
        | "url"
        | "time"
        | "date"
        | undefined;
}>()

/**
 * Create a default value ref
 */
const value = $ref('')

/**
 * Define an emit for updating the v-model in the parent
 */
const emit = defineEmits(['update:modelValue'])

/**
 * Create a function to take in the new value and emit it
 * 
 * @param value new model value
 */
function emitInput(value: number | Date | string | null): void {
    emit('update:modelValue', value)
}

function required (value: any) {
    return (value.length < 1) ? '' : true
}
</script>

<template>
    <div class="q-px-xs">
        <div class="row">
            <div class="col">
                <q-input
                    :ref="name"
                    v-model="value"
                    :type="type"
                    square
                    filled
                    stack-label
                    value=""
                    :label="label"
                    :rules="[required]"
                    :mask="mask"
                    fill-mask
                    unmasked-value
                    @update:model-value="(value) => { emitInput(value) }"
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