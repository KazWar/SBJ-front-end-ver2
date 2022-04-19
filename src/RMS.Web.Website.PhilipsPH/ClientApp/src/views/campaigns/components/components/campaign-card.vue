<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { Navigate } from '@/common/utilities'

const { campaign } = defineProps<{
    campaign:Campaign
}>()

const { name, code, description, thumbnail, startDate, endDate } = campaign

//* Initialize i18n for the 'i18n-t' tag
useI18n({})

</script>

<template>
    <q-card 
        v-ripple 
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
                    <i18n-t keypath="campaign-card.operational-date" tag="p">
                        <template #startDate>
                            <span class="text-subtitle2">
                                {{ new Date(startDate).toDateString() }}
                            </span>
                        </template>
                        <template #endDate>
                            <span class="text-subtitle2">
                                {{ new Date(endDate).toDateString() }}
                            </span>
                        </template>
                    </i18n-t> 
                </div>

                <q-separator/>
                
                <div class="text-body1">
                    <!-- eslint-disable-next-line vue/no-v-html -->
                    <div v-html="description"></div>
                </div>
            </q-card-section>

            <!-- If thumbnail path is empty/null, don't show thumbnail -->
            <q-img
                v-if="thumbnail"
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

<i18n src="./campaign-card.translations.json"/>