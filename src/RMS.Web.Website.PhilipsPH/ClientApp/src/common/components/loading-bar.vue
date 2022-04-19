<script lang="ts" setup> 

</script>

<template>
    <div class="row justify-center">
        <div>
            <div class="col">
                <p>
                    Loading languages <q-spinner-dots/>
                </p>
            </div>
            <div class="col">
                <q-circular-progress
                    indeterminate
                    size="100px"
                    :thickness="0.25"
                    color="cyan-3"
                    track-color="grey-3"
                    class="q-ma-md"
                />
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>