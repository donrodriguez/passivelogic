<script setup lang="ts">

</script>

<template>
  <div class="load"></div>
</template>

<style scoped>
@keyframes rotate {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

@-webkit-keyframes rotate {
  from {
    -webkit-transform: rotate(0deg);
  }
  to {
    -webkit-transform: rotate(360deg);
  }
}

.load {
  width: 20px;
  height: 20px;
  border: solid 2.5px var(--color-accent-2);
  border-radius: 50%;
  border-right-color: transparent;
  border-bottom-color: transparent;
  -webkit-transition: all 0.5s ease-in;
  -webkit-animation-name:             rotate;
  -webkit-animation-duration:         1.0s;
  -webkit-animation-iteration-count:  infinite;
  -webkit-animation-timing-function: linear;

  transition: all 0.5s ease-in;
  animation-name:             rotate;
  animation-duration:         1.0s;
  animation-iteration-count:  infinite;
  animation-timing-function: linear;
}
</style>