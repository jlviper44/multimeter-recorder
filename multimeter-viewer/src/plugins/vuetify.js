import Vue from 'vue';
import Vuetify from 'vuetify/lib/framework';

Vue.use(Vuetify);

export default new Vuetify({
    iconfont: 'md',
    theme: {
        options: { customProperties: true },
        themes: {
            light: {
                primary: '#003366',
                secondary: '#1564BF',
                accent: '#cce5ff',
                border: "#ffffff"
                
            }
        }
    }
});