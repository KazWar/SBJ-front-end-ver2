<script setup lang="ts">
//* External library
import { useI18n } from 'vue-i18n'
import { useRoute } from 'vue-router'
import { Form } from '@/common/models'
import { storeToRefs } from 'pinia'
import { onMounted } from 'vue'
//* Components
import FormStepper from './components/form-stepper.vue'
//* Stores
import { useCampaignStore, useFormStore } from '@/stores'

//* Access the navigated route to obtain url parameters
const { params } = useRoute()

//* Explicitly create an i18n instance using the global scope to get the global locale
const { locale } = $(useI18n({ useScope: "global" }))

//* Get the required methods from the stores
const { getCampaignByCode } = useCampaignStore()
const { loadForm } = useFormStore()
const { item } = storeToRefs(useFormStore())

/**
 * Primary Key ID off the selected campaign
 */
const id:number = getCampaignByCode(Number(params.campaignCode)).id

let loaded = $ref(false)

onMounted(async () => {
  loaded = await loadForm(String(locale), id) as boolean
})
</script>

<template>
    <div class="row registration-form">
      <q-page class="offset-md-3 col-6 ">
        <form-stepper v-if="loaded" :form="(item as Form)"/>
      </q-page>
    </div>
</template>

<style scoped lang="scss">
.registration-form{
  padding-top: 40px;
  padding-bottom: 50px;
}</style>