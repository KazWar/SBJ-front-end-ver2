<script setup lang="ts">
import { Navigate } from "@/common/utilities"
import { useI18n } from "vue-i18n"
import CountryFlag from "vue-country-flag-next"
import { useLocaleStore } from '@/stores'
import { Ref, ref }  from "vue"

const { RequireLocales } = useLocaleStore()

//* Setting reactive properties for the component
const selection: Ref<Locale | undefined> = ref()

//* Explicitly create an i18n instance using the global scope to adjust locale
let { locale } = useI18n({ useScope: "global" })

//* helper function for changing the locale
function ChangeLocale(selection:any) {
  locale.value = selection.code
}

//* Get the options
const options:any = await RequireLocales()

</script>

<template>
  <q-select
    name="LocaleSelector"
    v-model="selection"
    outlined
    emit-value
    map-options
    square
    :options="options"
    label="Pick your locale...."
    option-label="name"
    @update:model-value="
        //* Set the global i18n Locale to the selection
        ChangeLocale(selection),

        //* Navigate to the campaign overview page
        Navigate({
          name: 'Campaigns',
          params: { locale: selection!.code },
        })
      "
    >
    <!-- Custom icon slot for country flags -->
    <template #option="scope">
      <q-item v-bind="scope.itemProps">
        <q-item-section avatar>
          <!-- remove this code hack with better locale implementation -->
          <country-flag
            :country="scope.opt.countryCode"
          />
        </q-item-section>
        <q-item-section>
          <q-item-label>{{ scope.opt.name }}</q-item-label>
        </q-item-section>
      </q-item>
    </template>
  </q-select>
</template>

<style scoped lang="scss">
.normal-flag {
  //* override default flag styling
  margin: -0.5em -1em -0.5em -1em !important;
}
</style>