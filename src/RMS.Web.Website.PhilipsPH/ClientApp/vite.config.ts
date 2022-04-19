/// <reference types="vitest" />

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import eslintPlugin from 'vite-plugin-eslint'
import { quasar, transformAssetUrls } from '@quasar/vite-plugin'
// import path from 'path'

//* This dependancy was installed with --force due version mismatch
import vueI18n from '@intlify/vite-plugin-vue-i18n'

export default defineConfig({
  plugins: [
    vue({
      reactivityTransform: true,
      template: { 
          transformAssetUrls 
        }
    }),

    quasar({
      autoImportComponentCase: 'combined',
      sassVariables: 'src/common/styling/quasar-variables.sass'
    }),
    
    vueI18n({
      runtimeOnly:false,

      //* If translations are used outside of the views, uncomment here and in the plugin.
      //include: path.resolve(__dirname, './src/data/locales/**'),
      compositionOnly: true
    }),
    
    eslintPlugin()
  ],

  resolve: {
		alias: {
			// Defines where imports starting from @/ prefix are found
			'@/': __dirname + '/src/',
      'vue-i18n': 'vue-i18n/dist/vue-i18n.runtime.esm-bundler.js'
		},
	},

  test:{
    globals: true,
    environment: 'jsdom', 
    coverage: {
      reporter: ['text', 'json', 'html']
    }
  }

  // For dist debugging, comment/uncomment as needed
  // build: {
  //   minify:false,
  //   sourcemap:true
  // }
})