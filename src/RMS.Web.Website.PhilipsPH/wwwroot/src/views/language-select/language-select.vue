<script setup lang="ts">
import LocaleSelector from './components/locale-selector.vue'
import LoadingBar from '@/common/components/loading-bar.vue'
</script>

<template>
  <q-page-container class="page-container-locale-selector">
    <div class="row">
      <q-page class="offset-md-3 col-6 ">
        <suspense>
          <!-- Language selection component -->
          <template #default>
            <locale-selector/>
          </template>

          <!-- Loading animation -->
          <template #fallback>
            <loading-bar/>
          </template>
        </suspense>
      </q-page>
    </div>
  </q-page-container>
</template>

<style scoped lang="scss">

.page-container-locale-selector{
  padding: 10px 10px 10px 20px !important
}
</style>
