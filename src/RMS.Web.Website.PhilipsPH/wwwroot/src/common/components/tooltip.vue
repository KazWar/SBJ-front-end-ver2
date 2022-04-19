<script lang="ts">
  export default {
    name: 'ToolTip'
  }
</script>

<script setup lang=ts>
const { icon, text, color } = defineProps<{
        text:string,
        icon?:string,
        color?:string
}>()

</script>

<template>
    <q-icon
        size="1.2em"
        :name="(icon ? icon : 'help_center')" 
        :color="(color) ? color : 'text-blue-grey-6'"
    >
        <q-tooltip>
            {{text}}
        </q-tooltip>
    </q-icon>
</template>

<style lang="scss" scoped></style>