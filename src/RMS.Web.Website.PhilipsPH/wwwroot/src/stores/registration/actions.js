import { useFormService } from "@/services";
import { useRegistrationStore } from "@/stores";
export default {
    async postRegistration(locale, campaignCode) {
        const { PostForm } = useFormService();
        const { rawData } = useRegistrationStore();
        try {
            PostForm(locale, campaignCode, rawData);
        }
        catch (error) {
            return error;
        }
    }
};
//# sourceMappingURL=actions.js.map