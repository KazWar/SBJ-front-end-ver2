<script lang="ts">
  export default {
    name: 'CheckBox'
  }
</script>

<script setup lang=ts>

const { label, icon, name } = defineProps<{
    label:string
    name:string
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

/**
 * Create a function to take in the new value and emit it
 * 
 * @param value new model value
 */
function emitInput(value: boolean): void {
    emit('update:modelValue', String(value))
}

function required (value: any) {
    return (value === null || value === '') ? false : true
}
</script>

<template>
    <div class="q-px-xs">
        <div class="row">
            <div class="col">
                <q-checkbox
                    :ref="name"
                    v-model="value"
                    :label="label"
                    checked-icon="task_alt"
                    unchecked-icon="highlight_off"
                    :rules="[required]"
                    @update:model-value="(value:boolean) => { emitInput(value) }"
                />
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>