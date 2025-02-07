<template>
  <div>
    <h1>Reports</h1>
    <button @click="generateReport">Generate Report</button>
  </div>
</template>

<script setup>
const generateReport = () => alert('Report Generated');
</script>