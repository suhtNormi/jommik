<template>
    <div class="timer-box">
        <div class="time-display">
        <span class="icon">⏱️</span>
        <p>{{ minutes }}:{{ seconds < 10 ? '0' : '' }}{{ seconds }}</p>
        </div>
        <button @click="toggleTimer">
        {{ isRunning ? "⏸️" : (minutes > 0 || seconds > 0 ? "▶️" : "Alusta") }}
        </button>
        <button @click="confirmTime" :disabled="minutes === 0 && seconds === 0">
        Lõpeta
        </button>
        <p v-if="savedTime">Treeningu kestus: {{ savedTime }}</p>
    </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, defineEmits } from 'vue';
 
const minutes = ref(0);
const seconds = ref(0);
const isRunning = ref(false);
const savedTime = ref(null);
let timerInterval = null;
const emit = defineEmits(['endTime']);


const startTimer = () => {
    isRunning.value = true;
    timerInterval = setInterval(() => {
    seconds.value++;
    if (seconds.value === 60) {
    seconds.value = 0;
    minutes.value++;
    }
    }, 1000);
};

const stopTimer = () => {
    isRunning.value = false;
    clearInterval(timerInterval);
    timerInterval = null;
};

const toggleTimer = () => {
    isRunning.value ? stopTimer() : startTimer();
};

const confirmTime = () => {
    savedTime.value = `${minutes.value}:${seconds.value < 10 ? '0' : ''}${seconds.value}`;
    const now = new Date().toISOString().slice(0, 16).replace('T', ' ');
    emit('endTime', now); // lisa et workoutevent saaks lõppaja teada
    minutes.value = 0;
    seconds.value = 0;
    stopTimer(); 
};

onMounted(() => { //lehe laadimisel taimer käima
  startTimer();
});

onUnmounted(() => {
  stopTimer(); //taimer kinni
});

</script>

<style scoped>
@import '../designs/timer.css';
</style>
  