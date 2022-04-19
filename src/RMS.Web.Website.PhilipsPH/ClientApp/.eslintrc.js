module.exports = {
  root: true,

  parserOptions: {
    parser: "@typescript-eslint/parser",
    extraFileExtensions: [ '.vue' ]
  },

  plugins: [
    "@typescript-eslint"
  ],

  env: {
    browser: true,
    es2021: true,
    node: true,

    // To prevent the linter from highlighting the $ and $$ ref (un)wrapping as incorrect
    "vue/setup-compiler-macros": true
  },

  // Rules order is important, please avoid shuffling them
  extends: [
    // Base ESLint recommended rules
    // 'eslint:recommended',

    // https://github.com/typescript-eslint/typescript-eslint/tree/master/packages/eslint-plugin#usage
    // ESLint typescript rules
    'plugin:@typescript-eslint/recommended',

    // Uncomment any of the lines below to choose desired strictness,
    // but leave only one uncommented!
    // See https://eslint.vuejs.org/rules/#available-rules
    'plugin:vue/vue3-essential', // Priority A: Essential (Error Prevention)
    // 'plugin:vue/vue3-strongly-recommended', // Priority B: Strongly Recommended (Improving Readability)
    // 'plugin:vue/vue3-recommended', // Priority C: Recommended (Minimizing Arbitrary Choices and Cognitive Overhead)

    // https://github.com/prettier/eslint-config-prettier#installation
    // usage with Prettier, provided by 'eslint-config-prettier'.
    'prettier'
  ],

  plugins: [
    // required to apply rules which need type information
    '@typescript-eslint',

    // https://eslint.vuejs.org/user-guide/#why-doesn-t-it-work-on-vue-files
    // required to lint *.vue files
    'vue',

    'eslint-plugin-tsdoc'
    
    // https://github.com/typescript-eslint/typescript-eslint/issues/389#issuecomment-509292674
    // Prettier has not been included as plugin to avoid performance impact
    // add it as an extension for your IDE
    
  ],

  globals: {
  },

  rules: {
    
    '@typescript-eslint/explicit-function-return-type': 'off',
    'prefer-promise-reject-errors': 'off',
    "vue/script-setup-uses-vars": "error",
    "vue/no-setup-props-destructure":"off",
    "vue/valid-v-on":"warn",
    "vue/multi-word-component-names": ["warn", {
      "ignores": [
        "About",
        "Error",
        "Footer"
      ]
    }],
    "@typescript-eslint/no-explicit-any": "off",
    "@typescript-eslint/no-use-before-define": ["error", { "variables": false }],
    "@typescript-eslint/no-empty-function": ["error"],

    // this rule, if on, would require explicit return type on the `render` function
    '@typescript-eslint/explicit-function-return-type': 'off',

    // The core 'no-unused-vars' rules (in the eslint:recommeded ruleset)
    // does not work with type definitions
    'no-unused-vars': 'off',

    // Base lint rule needs to be turned off as it conflicts with the TS lint version
    "no-use-before-define": "off",
    "no-undef": "off",
    "no-empty":"warn",

    // Don't use semi-colons, the language doesn't require it and it doesn't provide more clarity.
    "semi": ["warn", "never"],

    // Base lint rule needs to be turned off as it conflicts with the TS lint version
    "no-empty-function": "off",

    // To enable the TSdoc plugin
    "tsdoc/syntax": "warn",

    // allow debugger during development only
    'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off'
  }
}