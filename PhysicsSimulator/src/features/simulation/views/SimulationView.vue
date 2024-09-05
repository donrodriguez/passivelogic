<script setup lang="ts">

</script>

<template>
  <div class="container">
    <h1>Hello</h1>
    <div class="grid w-full max-w-sm items-center gap-1.5">
      <Label for="email">Email</Label>
      <Input id="email" type="email" placeholder="Email" />
    </div>
  </div>

</template>

<style scoped>
.container {
  width: 100vw;
  height: 100vh;

}

h1 {
  color: red;
}
</style>