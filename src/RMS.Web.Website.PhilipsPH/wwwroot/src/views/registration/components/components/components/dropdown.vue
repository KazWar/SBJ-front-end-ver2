<script lang="ts">
  export default {
    name: 'DropdownMenu'
  }
</script>

<script setup lang=ts>
import { Value } from '@/common'
import { useRegistrationStore } from '@/stores'
import { storeToRefs } from 'pinia'

let { label, options, icon, useInput, po } = defineProps<{
        label:string
        icon?:string | undefined
        options:Array<Value>
        po?:Array<any> | undefined
        useInput:boolean
}>()

const { rawData } = storeToRefs(useRegistrationStore())


/**
 * Create a default value ref
 */
const value = $ref(null)
const value2 = $ref(null)
let selectOptions:any= $ref(options)
let giftOptions:any= $ref(po)

/**
 * Define an emit for updating the v-model in the parent
 */
const emit = defineEmits(['update:modelValue'])

/**
 * Create a function to take in the new value and emit it
 * 
 * @param value new model value
 */
function emitInput(value: string | object | number): void {
    emit('update:modelValue', value)
}


function filterFn (val:any , update:any):void {
    setTimeout(() => {
        update(() => {
              if (val === '') {
                giftOptions = options
              }
              else {
                const needle = val.toLowerCase()
                giftOptions = options.filter((v:any) => v.value.toLowerCase().indexOf(needle) > -1)
              }
        },

        //* ref refers to the component itself
        (ref:any) => {
            //* Trick to automatically select the first result from filterered results
            if (val !== '' && ref.options.length > 0) {
            ref.setOptionIndex(-1)
            ref.moveOptionSelection(1, true)
            }
        })
    }, 100)
}

function filterGifts (val:any , update:any):void {
    update(() => {
        if (val === null){
            giftOptions = po
        } else {
            giftOptions = po?.filter((v:any) => v.ChosenItemId === (value as any).key)
        }
    })
}

function required (ref: any) {
    return (ref === null || ref === '') ? false : true
}
</script>

<template>
    <div class="q-px-xs">
        <div class="row">
            <div class="col">
                <q-select
                    v-model="value"
                    class="dropdown"
                    square
                    filled
                    stack-label
                    :label="label"
                    :options="selectOptions"
                    option-value="key"
                    option-label="value"
                    map-options
                    :use-input="useInput"
                    clearable
                    input-debounce="0"
                    :rules="[required]"
                    @filter="filterFn"
                    @update:model-value="(value) => { emitInput(value) }"
                >
                    <template #after>
                        <tool-tip :icon="icon" text="hello"/>
                    </template>
                </q-select>
            </div>
        </div>
        <div v-if="(po as Array<any>)" class="row">
            <div class="col">
                <q-select
                    v-model="value2"
                    class="dropdown"
                    :disable="value == null"
                    square
                    filled
                    :options="giftOptions"
                    option-value="HandlingLineId"
                    option-label="HandlingLineDescription"
                    clearable
                    map-options
                    input-debounce="0"
                    :rules="[required]"
                    @filter="filterGifts"
                >
                    <template #after>
                        <tool-tip :icon="icon" text="hello"/>
                    </template>
                </q-select>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.dropdown {
    // Apparantly the size for dropdown components isn't the same as for inputs 60 for input and 40 for dropdowns
    height: 76px;
}

</style>