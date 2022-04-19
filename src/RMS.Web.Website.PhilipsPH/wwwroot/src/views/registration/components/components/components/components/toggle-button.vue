<script setup lang=ts>

const { fileUrl, toggle } = defineProps<{
        fileUrl:string
        toggle:boolean
}>()

/**
 * Define an emit for updating the v-model in the parent
 */
const emit = defineEmits(['toggled'])

</script>


<template>
    <q-toggle
        :model-value="toggle"
        :disable="(fileUrl) ? false : true"
        checked-icon="visibility"
        color="green"
        unchecked-icon="visibility_off"
        size="lg"
        @update:model-value="value => emit('toggled', value)"
    >
        <q-tooltip> Shows or hides uploaded file(s) </q-tooltip>
    </q-toggle>
</template>

<style lang="scss" scoped></style>