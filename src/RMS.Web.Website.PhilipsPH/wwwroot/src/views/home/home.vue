<script setup lang="ts">
import { useConfigurationStore } from '@/stores'
import { useRouter } from 'vue-router'

const { settings } = useConfigurationStore()
const router = useRouter()

//* Check if the overview is enabled
const companyOverviewEnabled:any = settings.campaignOverview.isEnabled
if (companyOverviewEnabled) router.push('locale')
</script>

<template>
 <div>
    <p>Redirecting to language selection...</p>
    <p>Please wait or if you're not being redirected 
        Navigate with the link below.
    </p>
    <router-link to='/locale'>Go to language selection</router-link>
  </div>
</template>

<style scoped lang="scss"></style>