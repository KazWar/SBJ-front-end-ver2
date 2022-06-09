<script setup lang="ts">
//* External Libraries
import { createInput } from "@formkit/vue"
import { Bundle, Field } from "@/common"

//* Components
import InputText from './components/input.vue'

const { name, title, bundles } = defineProps<{
  name: string,
  title: string,
  bundles:Bundle[]
}>()

//! Should probably become a helper function
let component:any = (name:string) => {
  //! Place it in a store
  const components:Record<string,object> = {
    'InputText':  InputText
  }

  return createInput(components[name], { props: ['field']})
}

</script>

<template>
  <q-step
    :name="name"
    :title="name"
  >
    <form-kit type="group">
      <!-- Loop over all the bundles in a section -->
      <template v-for="(bundle, index) in bundles" :key="index">
        <!-- Loop over all the fields in a bundle -->
        <template v-for="(fields, index) in bundle" :key="index">
          <!-- Loop over each field in fields -->
          <template v-for="(field, index) in fields" :key="index">
            <form-kit :type="component((field as Field).name)" :field="field"/>
          </template>
        </template>
      </template>
    </form-kit>
  </q-step>
</template>

<style scoped lang="scss"></style>