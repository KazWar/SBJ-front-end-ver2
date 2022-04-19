<script setup lang="ts">
</script>

<template>
  <!-- Quasar layout configuration -->
  <q-layout view="hhh lpR fFf">

    <!-- Header -->
    <q-header bordered class="bg-none">
        <div class="header-img"/>
    </q-header>

    <!-- Body content -->
    <q-page-container>
        <router-view />
    </q-page-container>

    <!-- Footer -->
    <q-footer class="bg-grey-8 text-white">
    </q-footer>
  </q-layout>
</template>

<style lang="scss">
@font-face {
  font-family: NeueFrutigerWorld;
  src: url("@/assets/fonts/NeueFrutigerWorld-regular.ttf");
}


body {
  color:$philips-blue-5;
  font-family:'NeueFrutigerWorld' !important;
}

.bg-none{
  background: none !important;
}

.header-img {
  background-image: url("@/assets/bground-header5-p-2600.jpeg");
  background-repeat: no-repeat;
  background-size: cover;
  background-position: 60% 0%;
  height: 400px;
  width: 100%;
}

.logo-primary{
  /* Overwrites q-img position:relative */
  position: absolute !important;
  left: 0px;
  margin-top: 10px;
  margin-left: 2rem;
  max-width: 150px;
}

.makita-logo{
  padding-top:1rem;
}

</style>
