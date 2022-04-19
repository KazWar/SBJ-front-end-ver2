<script setup lang="ts">
import CampaignCard from './components/campaign-card.vue'
import { useCampaignStore } from '@/stores'
import { useI18n } from 'vue-i18n'

//* Destructure the parameterized getter from the store
const { RequireCampaigns } = useCampaignStore()

//* Explicitly create an i18n instance using the global scope to get the globally set locale
const { locale } = $(useI18n({ useScope: "global" }))

//* Get the campaigns
const campaigns = await RequireCampaigns(String(locale))
</script>

<template>
    <template v-for="(campaign, index) in campaigns" :key="index">
        <campaign-card :campaign="campaign"/>
    </template>
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