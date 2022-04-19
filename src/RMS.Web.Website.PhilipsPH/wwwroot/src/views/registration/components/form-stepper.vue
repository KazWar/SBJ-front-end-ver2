<script setup lang="ts">

//* Components
import Step from './components/step.vue'
import SummaryStep from './components/summary-step.vue'

//* Models
import { Form } from '@/common'
import { QStepper } from 'quasar'
import { useRegistrationStore } from '@/stores'
import { storeToRefs } from 'pinia'
import { useRoute, useRouter } from 'vue-router'


const { form } = defineProps<{
    form:Form
}>()

const { sections } = form
const { rawData } = storeToRefs(useRegistrationStore())
const { postRegistration } = useRegistrationStore()
const { params } = useRoute()
const { push } = useRouter()


/**
 * Current position in the stepper, AKA which section of the form you're in
 */
let current: any = $ref("0")

/**
 * Number of sections
 */
const SectionCount:number = sections.length

/**
 * Ref declaration
 */
const stepper:QStepper = $ref()


function nextStep():void{
    stepper.next()
}

function prevStep():void{
    stepper.previous()
}

function register(){
    (rawData.value as any)['CampaignCode'] = params.campaignCode;
    (rawData.value as any)['Locale'] = params.locale
    postRegistration(params.locale, Number(params.campaignCode))

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

</script>

<template>
    <q-stepper
        ref=stepper
        v-model="current"
        keep-alive
        color="primary"
        done-color="green"
        animated
    >
        <template v-for="({ fields, name }, index) in sections" :key="index" >
            <step
                :ref="`${current}`"
                :name="index.toString()"
                :title="name"
                :prefix="index+1"
                icon="perm_identity"
                :fields="fields"
                :done="current > index"
            />
        </template>

        <q-step
            :ref="`${current}`"
            :name="String(SectionCount)"
            title="Summary"
            icon="done_all"
        >
            <summary-step/>
        </q-step>

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
</template>

<style scoped lang="scss">
.registration-form{
  padding-top: 40px;
  padding-bottom: 50px;
}
</style>