<script setup lang=ts>
const { fileUrl } = defineProps<{
        fileUrl:string
}>()

/**
 * Opens the clicked file in a new tab
 */
const openFile = ():void => {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    (window as Window).open(fileUrl, '_blank')!.focus()
    console.log(fileUrl)
}
</script>


<template>
    <div class="q-pa-sm">
        <q-card square class="shadow-1 preview-card cursor-pointer" @click="openFile">
            <q-img
                width="250px"
                height="150px"
                :src="fileUrl"
                fit="cover"
            >
                <template #error >
                    <q-icon
                        style="background-color: whitesmoke;"
                        color="red"
                        name="picture_as_pdf"
                        size="11em"
                    />
                </template>
            </q-img>
        </q-card> 
    </div>
</template>

<style lang="scss" scoped>

.preview-card {
    &:hover{
        &::after{
            background-color:rgba($color: #000000, $alpha: 0.3);
            padding: 4px 8px;
            position: absolute;
            content: "🔍";
            top: 0;
            right: 0 ;
            bottom: 0;
            left: 0;
        }
    }
}
</style>