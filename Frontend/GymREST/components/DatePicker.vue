<script setup lang="ts">
import { DatePicker as VCalendarDatePicker } from 'v-calendar'
// @ts-ignore
import type { DatePickerDate, DatePickerRangeObject } from 'v-calendar/dist/types/src/use/datePicker'
import 'v-calendar/dist/style.css'

defineOptions({
  inheritAttrs: false
})

const props = defineProps({
  modelValue: {
    type: [Date, Object] as PropType<DatePickerDate | DatePickerRangeObject | null>,
    default: null
  }
})

const emit = defineEmits(['update:model-value', 'close'])

const date = computed({
  get: () => props.modelValue,
  set: (value) => {
    emit('update:model-value', value)
    emit('close')
  }
})

const attrs = {
  'transparent': true,
  'borderless': true,
  'color': 'primary',
  'is-dark': { selector: 'html', darkClass: 'dark' },
  'first-day-of-week': 2
}

function onDayClick(_: any, event: MouseEvent): void {
  const target = event.target as HTMLElement
  target.blur()
}
</script>

<template>
  <VCalendarDatePicker
    v-if="date && (date as DatePickerRangeObject)?.start && (date as DatePickerRangeObject)?.end"
    v-model.range="date"
    :columns="2"
    v-bind="{ ...attrs, ...$attrs }"
    @dayclick="onDayClick"
  />
  <VCalendarDatePicker
    v-else
    v-model="date"
    v-bind="{ ...attrs, ...$attrs }"
    @dayclick="onDayClick"
  />
</template>

<style>
:root {
  --color-primary-50: 211, 85, 226;   /* Light Purple */
  --color-primary-100: 182, 64, 203;  /* Slightly darker purple */
  --color-primary-200: 155, 45, 180;  /* Darker purple */
  --color-primary-300: 128, 27, 157;  /* Even darker purple */
  --color-primary-400: 101, 8, 134;   /* Deep purple */
  --color-primary-500: 75, 0, 111;    /* Very deep purple */
  --color-primary-600: 60, 0, 95;     /* Darker shade */
  --color-primary-700: 45, 0, 79;     /* Even darker purple */
  --color-primary-800: 30, 0, 63;     /* Very dark purple */
  --color-primary-900: 15, 0, 47;     /* Deep purple */
}

.vc-primary {
  --vc-accent-50: rgb(var(--color-primary-50));
  --vc-accent-100: rgb(var(--color-primary-100));
  --vc-accent-200: rgb(var(--color-primary-200));
  --vc-accent-300: rgb(var(--color-primary-300));
  --vc-accent-400: rgb(var(--color-primary-400));
  --vc-accent-500: rgb(var(--color-primary-500));
  --vc-accent-600: rgb(var(--color-primary-600));
  --vc-accent-700: rgb(var(--color-primary-700));
  --vc-accent-800: rgb(var(--color-primary-800));
  --vc-accent-900: rgb(var(--color-primary-900));
}
</style>
