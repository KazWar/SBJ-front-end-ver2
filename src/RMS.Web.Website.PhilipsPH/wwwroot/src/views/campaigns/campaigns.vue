<script setup lang="ts">
import CampaignCards from './components/campaign-cards.vue'
import LoadingBar from '@/common/components/loading-bar.vue'
import { useI18n } from 'vue-i18n'

//* Translations initialization
const { t } = useI18n({})
</script>

<template>
  <div class="row available-campaigns">
    <q-page
      class="offset-md-3 col-7 " >
      <p class="text-h2 text-center">{{ t('campaigns.title') }}</p>
        <suspense>
          <!-- Campaign cards component -->
          <template #default>
            <campaign-cards/>
          </template>

          <!-- Loading animation -->
          <template #fallback>
            <loading-bar/>
          </template>
        </suspense>
    </q-page>
  </div>
</template>

<style scoped lang="scss">
.available-campaigns{
  padding-top: 40px;
  padding-bottom: 50px;
}
</style>

<i18n src="./campaigns.translations.json"/>