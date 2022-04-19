<script setup lang="ts">
import { Field, Section } from '@/common'
import { useRegistrationStore, useFormStore } from '@/stores'
import { storeToRefs } from 'pinia'
import UploadPreviewCard from './components/components/upload-preview-card.vue'

const { rawData } = $(storeToRefs(useRegistrationStore()))
const { item } = $(storeToRefs(useFormStore()))

let fields:Array<any> = [...item.sections.map((section:Section) => { return [...section.fields]})]
fields = Array.prototype.concat.apply([], fields)
const data = $ref(Object.fromEntries(Object.entries(rawData).filter(([_, v]) => v != null)))

const product = (giftId:number):any => {
    const field:Field = fields.find((field:Field) => field.name === "ProductPremium")
    const options:any = field.options
    console.log(options)
    return ''
}

const label = (fieldName:string):string | undefined => {
    const field:Field = fields.find((field:Field) => field.name === fieldName)
    return field.label
}

const value = (fieldName:string):any | undefined => {
    const test:any = Object.entries(data).find((pair:any) => pair[0] === fieldName)

    if(typeof test[1] === 'object'){

        if (Array.isArray(test[1])) {
            const item = test[1][0]

            return (item.HandlingLineDescription) ? item.HandlingLineDescription : test[1][0]
        }

        return  (test[1].value) ? test[1].value :
                (test[1].retailerAddress) ? test[1].retailerAddress :
                (test[1].iban) ? test[1].iban : test[1]
    }

    return test[1]
}

const openFile = (fileUrl:any):void => {
    var win:any = window.open()
    win.document.write('<iframe src="' + fileUrl  + '" frameborder="0" style="border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;" allowfullscreen></iframe>')

}
</script>

<template>
    <template v-for="(field, index) in fields" :key="index">
        <div class="q-pa-xs">
            <q-field
                v-if="!(field.name === 'InvoiceImagePath')"
                readonly
                :label="field.label"
                stack-label
                square
                filled
                dense
            >
            {{ (value(field.name) === 'true') ? 'Agreed' : value(field.name) }}
            </q-field>
            <template
                v-if="(field.name === 'InvoiceImagePath')"
            >
                <q-field
                    readonly
                    :label="field.label"
                    stack-label
                    square
                    filled
                    dense
                >
                    <div class="q-pa-sm">
                        <q-card square class="shadow-1 preview-card cursor-pointer" @click="openFile(value(field.name))">
                            <q-img
                                width="250px"
                                height="150px"
                                :src="value(field.name)"
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
                </q-field>
            </template>
        </div>
    </template>
</template>

<style scoped lang="scss">

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
