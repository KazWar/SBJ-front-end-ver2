import { Type } from "ts-toolbelt/out/Any/Type"

export type Form = {
    formId: number
    name: string
    description: string | undefined
    locale: string
    sections: Section[]
}

export type Section = {
    id: number
    name: string
    description: string | undefined
    bundles: Bundle[]
}

export type Bundle = {
    id: number
    name: string
    description: string | undefined
    multiples: number
    fields: Field[]
}

export class Field {
    id: number
    name: string
    type: fieldType
    description: string | undefined
    label: string | undefined
    tooltip: string | undefined
    destinationTypeId: number
    linkId: number | undefined
    rules:Rule[] | undefined
    design: string | undefined
    features: string | undefined
    mask: string | undefined
}

export type Rule = {
    /**
     * Rule name, must not contain spaces.
     */
    name: string

    /**
     * Contains int values, which help to set the rule floor/ceiling.
     */
    conditions: number[]
}

export type fieldType = {
    name: string
    description: string | undefined
    subType: string | undefined
}