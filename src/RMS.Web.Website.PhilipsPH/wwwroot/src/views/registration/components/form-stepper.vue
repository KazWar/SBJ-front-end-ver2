<script setup lang="ts">
//* Models
import { Form } from '@/common'

//* External libraries
import { useRoute, useRouter } from 'vue-router'

//* Components
import Step from './components/step.vue'
import SummaryStep from './components/summary-step.vue'
import { QStepper } from 'quasar'

//* Stores
import { useFormStore, useCampaignStore } from '@/stores'

//* Get the route params
const { params } = useRoute()

//* Get the required methods from the stores
const { RequireForm } = useFormStore()
const { GetCampaignByCode } = useCampaignStore()

//* Define variables
const formId:number = GetCampaignByCode(Number(params.campaignCode)).formId

//* Ref declarations
const stepper:QStepper = $ref()
let form:Form = $ref()
let formData = $ref({})

const response = await RequireForm(formId)

if (response === undefined){
// Future error logging implementation
    console.log(response)
} else {
    form = response as Form
}

/**
 * Destructure the form sections from the form
 */
const { sections } = form
console.log(sections)
const { push } = useRouter()

/**
 * Current position in the stepper, AKA which section of the form you're in
 */
let current: any = $ref("0")

/**
 * Number of sections
 */
const SectionCount:number = sections.length

function nextStep():void{
    stepper.next()
}

function prevStep():void{
    stepper.previous()
}

function register(){
    push({name:'ThankYou'})
}

function validate (ref:any) {
    const form = ref[current][0].$refs.form
    form.validate().then((success:any) => {
      if (success) {
          nextStep()
      }
      else {
          console.log(ref)
      }
    })
}

const submitHandler = async () => {
  // Let's pretend this is an ajax request:
  await new Promise((r) => setTimeout(r, 1000))
}

</script>

<template>
    <form-kit 
        type="form"
        v-model="formData"
        submit-label="Register"
        @submit="submitHandler"
    >
        <q-stepper
            ref=stepper
            v-model="current"
            keep-alive
            color="primary"
            done-color="green"
            animated
        >
            <!-- Loop over all the sections to create form steps -->
            <template v-for="(section, index) in sections" :key="index" > 
                <step
                    :ref="`${current}`"
                    :name="index.toString()"
                    :title="section.name"
                    :prefix="index+1"
                    icon="perm_identity"
                    :bundles="section.bundles"
                    :done="current > index"
                />
            </template>

            <!-- Stepper navigation, generates the back & forward buttons -->
            <template #navigation>
                <q-stepper-navigation>
                    <q-btn 
                        ref="stepper"
                        color="primary" 
                        square
                        :label="current == SectionCount ? 'Register' : 'Continue'"
                        @click="current == SectionCount ? register(): validate($refs)"
                    />
                    <q-btn 
                        v-if="current > 0" 
                        flat 
                        color="primary" 
                        square
                        label="Back" 
                        class="q-ml-sm"
                        @click="prevStep"
                    />
                </q-stepper-navigation>
            </template>
        </q-stepper>
    </form-kit>
</template>

<style scoped lang="scss">
.registration-form{
  padding-top: 40px;
  padding-bottom: 50px;
}
</style>