<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />

    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/login">Login</RouterLink>
    </nav>
  </header>

  <body>
    <RouterView />
  </body>
</template>

<style scoped>

header {
  position: fixed;
  top: 0;
  left: 0;
  padding: 1rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  //box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.1);
  z-index: 1000;
}

.logo {
  width: 80px;
  height: 80px;
  margin-bottom: 1rem;
}

nav {
  display: flex;
  flex-direction: column;
  text-align: center;
}

nav a {
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-bottom: 1px solid var(--color-border);
}

nav a:last-of-type {
  border-bottom: none;
}

nav a.router-link-exact-active {
  font-weight: bold;
  color: var(--color-text);
}

main {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  margin-left: 150px;
  text-align: center;
}
</style>
