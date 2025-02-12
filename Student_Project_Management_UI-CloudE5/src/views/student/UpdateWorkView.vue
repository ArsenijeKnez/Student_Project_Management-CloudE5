<script setup>
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import submissionService from "@/services/submissionService";
import DisplayWork from "@/components/DisplayWork.vue";
import UpdateWork from "@/components/student/UpdateWork.vue";

const route = useRoute();
const message = ref("");
const studentWork = ref(null);
const revert = ref(null);

const fetchStudentWork = async () => {
  const studentWorkId = route.query.id; 

  if (!studentWorkId) {
    message.value = "Student Work ID is missing.";
    return;
  }

  try {
    const response = await submissionService.getStudentWork(studentWorkId);

    if (response.status === 200) {
      studentWork.value = response.data;

    } else {
      message.value = response.data.message || "Failed to fetch student work.";
    }
  } catch (error) {
    message.value = "Failed to retrieve student work.";
  }
};

onMounted(fetchStudentWork);

const revertVersion = async () =>{
  const studentWorkId = route.query.id; 

  if (!studentWorkId || !revert) {
    message.value = "Revert version number is missing.";
    return;
  }

  try {
    const response = await submissionService.revertVersion(studentWorkId, revert.value);

    if (response.status === 200) {
      fetchStudentWork();

    } else {
      message.value = response.data.message || "Revert failed.";
    }
  } catch (error) {
    message.value = "Revert failed.";
  }
}
</script>

<template>
  <div>
    <h2>Student Work Details</h2> 
    <p v-if="message" class="error">{{ message }}</p>
    <div v-if="studentWork && !message">
      <DisplayWork :studentWork="studentWork" /> 
    </div>
    <div v-else>Loading...</div>
    <UpdateWork v-if="route.query && route.query.id" :studentWorkId="route.query.id"/>
    <div class="input-group">
    <label>Revert Version</label>
    <input
      type="number"
      v-model="revert"
      placeholder="version num"
    />
    <button @click="revertVersion">Revert</button>
  </div>
  </div>
</template>
