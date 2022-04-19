<script lang="ts">
  export default {
    name: 'IbanChecker'
  }
</script>

<script setup lang=ts>
import { RechnerResponse } from '@/common'
import { useIbanService } from '@/services'

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

const { ValidateIban } = useIbanService()

/**
 * Create a default value ref
 */
const value: string = $ref('')
let valid = $ref(false)
let ibanBic = {}

/**
 * Define an emit for updating the v-model in the parent
 */
const emit = defineEmits(['update:modelValue'])

/**
 * Create a function to take in the new value and emit it
 * 
 * @param value new model value
 */
function emitInput(value: any): void {
    if(!value){
        valid = false
    }
    emit('update:modelValue', ibanBic)
}

function required (value: string) {
    return (value.length < 1) ? '' : true
}

async function validIban (iban: string){
    if (iban){
        const response: RechnerResponse = await ValidateIban(iban)

        if (response.result === "passed"){
            valid = true
            ibanBic = { iban:response.iban, bic:response.bic_candidates[0].bic}

            emitInput(ibanBic)
            return true
        } else {
            valid = false
            return false
        }
    } else {
        valid = false
        return false
    }
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
                    :bg-color=" (valid) ? 'light-green-2' : '' "
                    square
                    filled
                    stack-label
                    value=""
                    :label="label"
                    :rules="[required, validIban]"
                    mask="AA## XXXX #### #### ##"
                    unmasked-value
                    debounce="600"
                    fill-mask
                    :lazy-rules=true
                    @update:model-value ="emitInput(value)"
                >
                    <template #after >
                        <tool-tip :icon="icon" text="hello"/>
                    </template>
                </q-input>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
</style>