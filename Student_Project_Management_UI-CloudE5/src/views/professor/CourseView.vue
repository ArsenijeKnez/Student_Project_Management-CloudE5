<script setup>
import { ref, onMounted } from 'vue'
import SubmissionService from '@/services/submissionService.js'

const courses = ref([])
const newCourse = ref('')
const errorMessage = ref('')

const fetchCourses = async () => {
  const response = await SubmissionService.getCourses()
  if (response.success !== false) {
    courses.value = response.data
  } else {
    errorMessage.value = response.message
  }
}

const addCourse = async () => {
  if (!newCourse.value) return
  try {
    await SubmissionService.addCourse(newCourse.value)
    newCourse.value = ''
    fetchCourses()
  } catch (error) {
    errorMessage.value = error.message || 'Failed to add course'
  }
}

const deleteCourse = async (course) => {
  try {
    await SubmissionService.deleteCourse(course.id)
    fetchCourses()
  } catch (error) {
    errorMessage.value = error.message || 'Failed to delete course'
  }
}

onMounted(fetchCourses)
</script>

<template>
  <div>
    <h2>Courses</h2>
    <p v-if="errorMessage" style="color: red">{{ errorMessage }}</p>
    <ul>
      <li v-for="course in courses" :key="course">
        {{ course.name }} <button @click="deleteCourse(course)">Delete</button>
      </li>
    </ul>
    <input v-model="newCourse" placeholder="New course name" />
    <button @click="addCourse">Add Course</button>
  </div>
</template>
