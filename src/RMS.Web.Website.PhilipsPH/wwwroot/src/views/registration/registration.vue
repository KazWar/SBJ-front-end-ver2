<script setup lang="ts">
import FormStepper from './components/form-stepper.vue'

</script>

<template>
    <div class="row registration-form">
      <q-page class="offset-md-3 col-6 ">
        <suspense>
          <!-- Form stepper component -->
          <template #default>
            <form-stepper/>
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
.registration-form{
  padding-top: 40px;
  padding-bottom: 50px;
}</style>