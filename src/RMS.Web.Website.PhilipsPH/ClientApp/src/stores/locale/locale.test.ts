import { describe, expect, test, vi, beforeAll, SpyInstance } from 'vitest'
import { createPinia, storeToRefs } from 'pinia'
import Actions from './actions'
import { useLocaleStore } from '.'

describe("Locale store action tests", () => {
  describe('FetchLocales tests', () => {
    describe('Fetches items from API & returns it', () => {
      //* Iniate Pinia instance
      const store = useLocaleStore(createPinia())

      //* Define variables common use
      let requireLocaleService:SpyInstance
      let FetchLocalesSpy:SpyInstance
      let response:Locale[] | Error
      
      //* Set the common use variables & create mocks
      beforeAll(() => {
        requireLocaleService = vi.spyOn(Actions, 'requireLocaleService')
          .mockImplementation(():any => {
            return {
              GetLocales: vi.fn().mockResolvedValue({ locales: [1,2,3] })
            } 
          }
        )

        FetchLocalesSpy = vi.spyOn(Actions, 'FetchLocales')
      })

      test('Store items has 0 elements', () => {
        expect(store.items).length(0)
      })
      
      test('FetchLocales has been called', async () => {
        response = await Actions.FetchLocales()
        expect(FetchLocalesSpy).toHaveBeenCalled()
      })

      test('RequireLocaleService has been called', () => {
        expect(requireLocaleService).toHaveBeenCalled()
      })

      test('API returned a response', () => {
        expect(response).toEqual([1,2,3])
      })

      test('Returned the locales', () => {
        expect(FetchLocalesSpy).toHaveReturnedWith([1,2,3])
      })
    })
  })

  describe('RequireLocales tests', () => {
    describe('Caches items & returns them', () => {
      //* Iniate Pinia instance
      const store = useLocaleStore(createPinia())

      //* Define variables common use
      let requireLocaleService:SpyInstance
      let requireLocaleStore:SpyInstance
      let RequireLocalesSpy:SpyInstance
      let response:Locale[] | Error
      
      //* Set the common use variables
      beforeAll(async() => {
        requireLocaleStore = vi.spyOn(Actions, 'requireLocaleStore')
          .mockImplementation(():any => {
            return storeToRefs(store)
          }
        )

        requireLocaleService = vi.spyOn(Actions, 'requireLocaleService')
          .mockImplementation(():any => {
            return {
              GetLocales: vi.fn().mockResolvedValue({ locales: [1,2,3] })
            } 
          }
        )

        RequireLocalesSpy = vi.spyOn(Actions, 'RequireLocales')
      })

      test('Store items has 0 elements', () => {
        expect(store.items).length(0)
      })
      
      test('RequireLocales has been called', async () => {
        response = await Actions.RequireLocales()
        expect(RequireLocalesSpy).toHaveBeenCalled()
      })

      test('RequireLocaleService has been called', () => {
        expect(requireLocaleService).toHaveBeenCalled()
      })

      test('API returned a response', () => {
        expect(response).toEqual([1,2,3])
      })
      
      test('RequireLocaleStore has been called', () => {
        expect(requireLocaleStore).toHaveBeenCalled()
      })
      
      test('API response has been stored', () => {
        expect(store.items).length(3)
      })

      test('Returned the locales', () => {
        expect(RequireLocalesSpy).toHaveReturnedWith([1,2,3])
      })
    })
  })
})