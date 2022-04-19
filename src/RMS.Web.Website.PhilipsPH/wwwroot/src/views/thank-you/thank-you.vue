<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { Navigate } from '@/common'

//* Translations initialization
const { t } = useI18n({})

const cards = [
  {
    label:"Stap 1",
    description:"U heeft zich ingeschreven voor deze actie!"
  },
  {
    label:"Stap 2",
    description:"U ontvangt van ons binnen één uur een automatische bevestings e-mail."
  },
  {
    label:"Stap 3",
    description:"Wij controleren uw gegevens binnen 4 weken en wanneer de inschrijving goedgekeurd is ontvangt u een goedkeurings-email."
  },
  {
    label:"Stap 4",
    description:"Na de goedkeuring verwerken wij uw inschrijving en ontvangt u binnen 4 weken uw cashback of cadeau."
  }
]

</script>

<template>
    <div class="row">
      <q-page class="offset-md-2 col-8 q-pa-md ">
        <q-card
          flat
          square
        >
          <!-- Thank you page headers -->
          <q-card-section>
            <p class="text-h4 bold-text">
              Hartelijk dank!
            </p>
            <p class="text-subtitle1 bold-text">
              Bedankt voor uw deelname aan deze actie
            </p>
            <p class="text-h5 bold-text">
              Dit kunt u van ons verwachten:
            </p>
          </q-card-section>
          
          <q-card-section>
            <!-- Thank you image -->
            <q-img
              src="@/assets/thank-you-steps.svg"
            />
            
            <!-- Registration steps -->
            <q-card-section horizontal row>
              <template v-for="(card, index) in cards" :key="index">
                <q-card-section class="col-3">
                  <p class="bold-text text-subtitle2">
                    {{card.label}}
                  </p>
                  <p>
                    {{card.description}}
                  </p>
                </q-card-section>
              </template>
            </q-card-section>

            <!-- Additional questions -->
            <q-card-section class="q-py-none">
              <p class="bold-text text-subtitle2">
                Nog vragen?
              </p>
              <p>
                U kunt gemakkelijk en snel antwoord vinden op uw vraag door de <a class="link" href="">veelgestelde vragen</a> te raadplegen. Als uw vraag er niet
                tussenstaat, dan helps onze klantenservice u graag verder. Kijk voor onze contactmogelijkheden en openingstijden op <a class="link" href="philips.be/contact">philips.be/contact</a>
              </p>

              <q-btn
                flat
                label="Ga terug naar Philips"
                class="philips-home-button"
                @click="Navigate({ name:'company'})"
              />
            </q-card-section>

          </q-card-section>
        </q-card>
      </q-page>
    </div>
</template>

<style scoped lang="scss">
@font-face {
  font-family: NeueFrutigerWorldBold;
  src: url("@/assets/fonts/NeueFrutigerWorld-Bold.ttf");
}

.bold-text {
  font-family:'NeueFrutigerWorldBold' !important;
}

.philips-home-button{
  color: white;
  background-color: $philips-blue-4;
}

.link {
  color: $philips-blue-3;
  text-decoration: none;
}
</style>

<i18n src="./thank-you.translations.json"/>