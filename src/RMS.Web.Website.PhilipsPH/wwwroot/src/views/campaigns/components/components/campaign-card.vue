<script setup lang="ts">
import { Navigate } from '@/common/utilities'

defineProps<{
    name:string
    code:number | undefined
    description:string | undefined
    startDate: Date  
    endDate: Date
    thumbnailUrl:string | undefined
    conditionsUrl:string | undefined
    bannerUrl:string | undefined
    category: Category
}>()

</script>

<template>
    <q-card
        class="campaign-card shadow-1 text-left cursor-pointer"
        square
        @click="Navigate({ 
          name: 'PurchaseRegistration',
          params: {
              campaignCode:code
          }
        })">
        <q-card-section horizontal>
            <q-card-section>
                <div class="text-h5 text-bold"> 
                    {{ name }}
                </div>
                
                <q-separator/>

                <div class="text-caption text-weight-medium">
                    {{ startDate.toDateString() }} to {{ endDate.toDateString() }}
                </div>

                <q-separator/>
                
                <div class="text-body1">
                    {{ description }}
                </div>
            </q-card-section>

            <!-- If thumbnail path is empty/null, don't show thumbnail -->
            <q-img
                v-if="thumbnailUrl"
                class="col-4"
                src="https://cdn.quasar.dev/img/parallax2.jpg"
            />
        </q-card-section>
    </q-card>
</template>

<style scoped lang="scss">
.campaign-card{
    background: #f0f0f0;
    margin-top: 20px;

    &:hover{
        background: #e4e4e4;
    }
}
</style>
