<script setup lang="ts">
//* External Libraries
import { BlitzForm } from "blitzar"
import { Field } from "@/common"
import { useRegistrationStore } from "@/stores"
import { storeToRefs } from "pinia"

//* Components
import DropdownMenu from './components/dropdown.vue'
import InputText from './components/input.vue'
import DatePicker from './components/date-picker.vue'
import FilePicker from './components/file-picker.vue'
import IbanChecker from './components/iban-checker.vue'
import RadioButton from './components/radio-button.vue'
import CheckBox from './components/check-box.vue'

const { name, title, icon, fields } = defineProps<{
  name: string, 
  title: string, 
  icon: string, 
  fields:Array<Field>
}>()

/**
 * Get the registration form from the store
 */
const { rawData } = storeToRefs(useRegistrationStore())

//! Should probably become a helper function
let component:any = (name:string) => {
  //! Place it in a store
  const components:Record<string,object> = {
    'InputText':  InputText,
    'DropdownMenu': DropdownMenu ,
    'DatePicker': DatePicker,
    'FileUploader': FilePicker,
    'IbanChecker': IbanChecker,
    'RetailerRadioButton': RadioButton,
    'CheckBox': CheckBox
  }

  return components[name]
}

//! Bussiness logic for form?
function toSchema(field:Field):Record<string,any>{
  return {
      id:field.name, 
      name: field.name,
      label: field.label,
      options: field.options,
      po: field.PO,
      useInput: true,
      component: component(field.type)
    }
}

const schema:any = [...fields.map((field:Field) => {
  return toSchema(field)
})]

</script>

<template>
  <q-step
    :name="name"
    :title="name"
  >
    <q-form ref="form">
      <blitz-form v-model="rawData" :schema="schema" :internal-labels="true"/>
    </q-form>
  </q-step>
</template>

<style scoped lang="scss"></style>