<template>
  <v-app>
    <v-app-bar
      app
      color="primary darken-1"
      dark
    >
      <div class="d-flex align-center">
        <v-icon
          left
        >
          mdi-lightning-bolt
        </v-icon>
        <h2>Multimeter Output GUI</h2>
      </div>
      <v-spacer></v-spacer>
    </v-app-bar>

    <v-main>
      <Main/>
    </v-main>
  </v-app>
</template>

<script>
import Main from './components/Main';

export default {
  name: 'App',

  components: {
    Main,
  },

  data: () => ({
    //
  }),
};
</script>
<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  text-align: center;
  background-color: var(--v-primary-base);
}
</style>
