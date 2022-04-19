<script lang="ts">
  export default {
    name: 'RadioButton'
  }
</script>

<script setup lang=ts>
import { DropdownListDto, Value } from '@/common'

let { label, options, icon, useInput,name } = defineProps<{
        label:string
        name:string
        icon?:string | undefined
        options:Array<any>
        useInput:boolean
}>()

/**
 * Create a default value ref
 */
const value = $ref(null)
let selectOptions:any= [
  ...options.map((option:DropdownListDto) => {
    return { label:option.retailerAddress, value:option.retailerLocationId }
  })
]



/**
 * Define an emit for updating the v-model in the parent
 */
const emit = defineEmits(['update:modelValue'])

/**
 * Create a function to take in the new value and emit it
 * 
 * @param value new model value
 */
function emitInput(value: string | object | number | null): void {
  const v = options.find((option:DropdownListDto) => option.retailerLocationId === value)

  emit('update:modelValue', v)
}

function required (value: any) {
    return (value === null) ? '' : true
}
</script>

<template>
    <div class="q-px-xs">
        <div class="row">
            <div class="col">
              <q-field
                v-model="value" 
                filled 
                square 
                :label="label" 
                stack-label 
                :rules="[required]"
                @update:model-value ="emitInput(value)"
              >
                <template #control>
                  <q-option-group
                      :ref="name"
                      v-model="value"
                      type="radio"
                      :options="selectOptions"
                      @update:model-value ="emitInput(value)"
                    >
                    <template #label="opt">
                      <div class="row items-center">
                        {{ opt.label }}
                      </div>
                    </template>
                    </q-option-group>
                </template>
              </q-field>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>

.label1{
    font-size: medium !important;
}

</style>