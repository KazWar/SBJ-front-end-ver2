<script lang="ts">
  export default {
    name: 'InputText'
  }
</script>

<script setup lang=ts>
import { Field } from '@/common';
import { FormKitFrameworkContext } from '@formkit/core'


const { context } = defineProps<{
    // eslint-disable-next-line @typescript-eslint/ban-types
    context: FormKitFrameworkContext
}>()

const { name, description, label } = context.field as Field

function handleInput(e:any) {
  context.node.input(e.target.value)
}

</script>

<template>
    <div class="q-px-xs">
        <div class="row">
            <div class="col">
                <q-input
                    :ref="name"
                    :value="context._value"
                    :type="type"
                    square
                    filled
                    stack-label
                    value=""
                    :label="label"
                    :rules="[required]"
                    @update:model-value="(value) => { emitInput(value) }"
                >
                    <template #after >
                        <tool-tip :icon="icon" text="hello"/>
                    </template>
                </q-input>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>