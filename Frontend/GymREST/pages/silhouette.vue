<template>
<div id="app">

  <UPopover :popper="{ placement: 'bottom-start' }">
  <UButton icon="i-heroicons-calendar-days-20-solid" class="bg-purple-700 text-white hover:bg-purple-500">
    {{ selected?.start ? format(selected.start, 'yyyy-MM-dd HH:mm') : 'Start Date' }} - 
    {{ selected?.end ? format(selected.end, 'yyyy-MM-dd HH:mm') : 'End Date' }}
  </UButton>
  <template #panel="{ close }">
    <div class="flex items-center sm:divide-x divide-gray-200 dark:divide-gray-800">
      <div class="hidden sm:flex flex-col py-4">
        <UButton
          v-for="(range, index) in ranges"
          :key="index"
          :label="range.label"
          color="purple"
          variant="ghost"
          class="rounded-none px-6"
          :class="[isRangeSelected(range.duration) ? 'bg-gray-100 dark:bg-gray-800' : 'hover:bg-gray-50 dark:hover:bg-gray-800/50']"
          truncate
          @click="selectRange(range.duration)"
        />
      </div>
      <DatePicker v-model="selected" @close="close" />
    </div>
  </template>
</UPopover>

    <div class="human-body">
      <HumanSilhouette 
        :selection.sync="selection"
        @click="handleClick"
        :disabled="disabled"
        :multiple="multiple"
        :body-colour="bodyColour"
        :selection-colour="selectionColour"
        :categoryColors="categoryColors.value"
        :show-parts="{
          'head': true,
          'face': true,
          'neck': true,
          'shoulder-left': true,
          'shoulder-right': true,
          'arm-left': true,
          'forearm-left': true,
          'arm-right': true,
          'forearm-right': true,
          'chest-left': true,
          'chest-right': true,
          'belly-left': true,
          'ribs-left': true,
          'belly-right': true,
          'belly': true,
          'ribs-right': true,
          'thigh-left': true,
          'innerthigh-left': true,
          'feet-left': true,
          'calf-left': true,
          'knee-left': true,
          'thigh-right': true,
          'genitalia': true,
          'innerthigh-right': true,
          'right-feet': true,
          'calf-right': true,
          'knee-right': true,
          'elbow-right': true,
          'hand-right': true,
          'elbow-left': true,
          'hands-left': true,
          'armback-left': true,
          'leg-left': true,
          'buttock': true,
          'loin': true,
          'column': true,
          'head-back': true,
          'nape': true,
          'armback-right': true,
          'leg-right': true,
          'back-right': true,
          'clavicule-right': true,
          'back-left': true,
          'clavicule-left': true,
        }"
      />
    </div>
    <br>
    Selection: {{selection}}
<br>
<br>
<strong>Options:</strong>
<br>
<div class="option-group">
  <label>
    <input type="radio" v-model="disabled" :value="true"> Disabled
  </label>
  <label>
    <input type="radio" v-model="disabled" :value="false"> Not Disabled
  </label>
</div>
<br>
<div class="option-group">
  <label>
    <input type="radio" v-model="multiple" :value="true"> Multiple
  </label>
  <label>
    <input type="radio" v-model="multiple" :value="false"> Single
  </label>
</div>
<br>
<label for="bodyColour"> Body Colour: </label>
<input id="bodyColour" type="text" v-model="bodyColour">
<br>
<label for="selectionColour"> Selection Colour: </label>
<input id="selectionColour" type="text" v-model="selectionColour">
  </div>

  
</template>


<script setup>
import { ref, watch } from 'vue';
import { sub, format, isSameDay } from 'date-fns';
import { useStatStore } from '#build/imports'; // This is fine inside setup

// Accessing the Nuxt instance is valid here
const { getExerciseFeedback } = useStatStore();

const selection = ref('');
const disabled = ref(false);
const multiple = ref(false);
const bodyColour = ref("black");
const selectionColour = ref("green");

// Calendar and range picker logic
const ranges = [
  { label: 'Viimased 7 päeva', duration: { days: 7 } },
  { label: 'Viimased 14 päeva', duration: { days: 14 } },
  { label: 'Viimased 30 päeva', duration: { days: 30 } },
  { label: 'Viimased 3 kuud', duration: { months: 3 } },
  { label: 'Viimased 6 kuud', duration: { months: 6 } },
  { label: 'Viimane aasta', duration: { years: 1 } }
];

const selected = ref({
  start: sub(new Date(), { days: 14 }), // Default range: last 14 days
  end: new Date()
});

function isRangeSelected(duration) {
  return isSameDay(selected.value.start, sub(new Date(), duration)) && isSameDay(selected.value.end, new Date());
}

function selectRange(duration) {
  selected.value = { start: sub(new Date(), duration), end: new Date() };
}

// Fetch exercise data
const trainingLoad = ref({});
const categoryColors = ref({});
const bodyPartCategories = {
  Legs: ['thigh-left', 'thigh-right', 'calf-left', 'calf-right', 'feet-left', 'right-feet', 'knee-left', 'knee-right', 'buttock'],
  UpperBody: ['chest-left', 'chest-right', 'belly-left', 'belly-right', 'belly', 'ribs-left', 'ribs-right'],
  Arms: ['arm-left', 'arm-right', 'forearm-left', 'forearm-right', 'hand-right', 'hands-left', 'elbow-left', 'elbow-right', 'armback-left', 'armback-right'],
  Core: ['belly', 'genitalia', 'innerthigh-left', 'innerthigh-right'],
  Shoulders: ['shoulder-left', 'shoulder-right'],
  Back: ['back-left', 'back-right', 'column', 'loin']
};

async function fetchExerciseData() {
  try {
    const { start, end } = selected.value;
    // console.log(start, end);
    if (!start || !end) {
      console.error("Start or end date is undefined");
      return;
    }
    const formattedStart = format(start, 'yyyy-MM-dd HH:mm');
    const formattedEnd = format(end, 'yyyy-MM-dd HH:mm');
    console.log("start time format",formattedStart);
    console.log("end time format", formattedEnd);

    // Pass the formatted dates to the API call
    const feedback = await getExerciseFeedback(formattedStart, formattedEnd); // API call with formatted strings
    calculateTrainingLoad(feedback);
  } catch (error) {
    console.error("Error fetching exercise data:", error);
  }
}

function calculateTrainingLoad(feedback) {
  const load = {};

  // Sum reps for each body part category
  feedback.forEach(({ bodypart, totalReps }) => {
    load[bodypart] = (load[bodypart] || 0) + totalReps;
  });

  trainingLoad.value = load;

  // Generate colors based on training load
  generateCategoryColors(load);
}

function generateCategoryColors(load) {
  console.log("Training load:", load);
  // Iterate over the body part categories and calculate colors based on reps
  for (const [category, reps] of Object.entries(load)) {
    // Calculate intensity: every 25 reps increases the intensity
    const intensityFactor = Math.min(reps / 25, 10); // Cap at 10 for maximum red
    const redValue = Math.round(192 + (intensityFactor * 6.3)); // Scale to deepen red
    const color = `rgba(${redValue}, 0, 0, 1)`; // Solid color (no transparency)
    categoryColors.value[category] = color;
  }
  console.log("Generated colors:", categoryColors.value);
}

function getCategoryStyle(partName) {
  for (const [category, parts] of Object.entries(bodyPartCategories)) {
    if (parts.includes(partName)) {
      return { fill: categoryColors.value[category] || bodyColour.value };
    }
  }
  return { fill: bodyColour.value };
}

// Watch selected range and fetch data
watch(selected, fetchExerciseData, { immediate: true });
</script>

<style scoped>
#app {
  display: flex;
  flex-direction: column;
  justify-content:  flex-start;
  align-items: center;
  height: 100vh; 
  padding: 20px;
}
.human-body {
  margin-top: 100px;
  width: 300px;
  height: 420px;
  margin-top: 20px;
}
.option-group {
  display: flex;
  gap: 10px; 
}
.option-group label {
  display: flex;
  align-items: center;
}
svg path {
    fill: inherit !important; /* Ensure dynamic fill is applied */
  }

  /* Optional: Add hover styling for better debugging */
  svg path:hover {
    fill-opacity: 0.8;
  }
</style>
