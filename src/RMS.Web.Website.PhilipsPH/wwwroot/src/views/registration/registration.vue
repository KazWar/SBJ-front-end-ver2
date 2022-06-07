<script setup lang="ts">
//* External library
import { useRoute } from 'vue-router'
//* Components
import FormStepper from './components/form-stepper.vue'
//* Stores
import { useFormStore, useCampaignStore } from '@/stores'

//* Get the route params
const { params } = useRoute()

//* Get the required methods from the stores
const { RequireForm } = useFormStore()
const { GetCampaignByCode } = useCampaignStore()

//* Define variables
const code = Number(params.campaignCode)
const formId:number = GetCampaignByCode(code).formId

let loaded: unknown = $ref()
loaded = await RequireForm(1)
</script>

<template>
    <div class="row registration-form">
      <q-page class="offset-md-3 col-6 ">
          stuff
        {{ loaded }}
      </q-page>
    </div>
</template>

<style scoped lang="scss">
.registration-form{
  padding-top: 40px;
  padding-bottom: 50px;
}</style>