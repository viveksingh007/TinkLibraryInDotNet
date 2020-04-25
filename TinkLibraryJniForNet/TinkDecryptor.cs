using java.lang;
using net.sf.jni4net;
using net.sf.jni4net.jni;
using System.Configuration;
using Sys = System;

namespace TinkLibraryJniForNet
{
    public class TinkDecryptor
    {
        private JNIEnv _jniEnv = null;
        private string _trustedSigningKeysJson = null;

        /// <summary>
        /// Create TinkDecryptor by creating JNI .Net bridge and preload trusted signing keys from Google servers
        /// </summary>
        public TinkDecryptor()
        {
            // create bridge using jni4net.j.jar in the same folder as jni4net.n.dll
            var bridgeSetup = new BridgeSetup();
            bridgeSetup.AddAllJarsClassPath("./tink_jars/"); // load libs
            _jniEnv = Bridge.CreateJVM(bridgeSetup); // create jvm

            // preload trusted signing keys from Google servers, cache into memory before performing any transactions
            Class googlePaymentsPublicKeysManager = _jniEnv.FindClass("com/google/crypto/tink/apps/paymentmethodtoken/GooglePaymentsPublicKeysManager");
            var env = ConfigurationManager.AppSettings["Trusted_Signing_Keys_Env"]; // environment of trusted signing keys from configs
            if (env != "INSTANCE_TEST" && env != "INSTANCE_PRODUCTION")
                throw new ConfigurationErrorsException("Setting Trusted_Signing_Keys_Env must be either INSTANCE_TEST or INSTANCE_PRODUCTION.");

            // initialize public key manager and load signing keys
            var publicKeyManager = googlePaymentsPublicKeysManager.GetFieldValue<Object>(env, "Lcom/google/crypto/tink/apps/paymentmethodtoken/GooglePaymentsPublicKeysManager;");
            publicKeyManager.Invoke("refreshInBackground", "()V");
            _trustedSigningKeysJson = publicKeyManager.Invoke<String>("getTrustedSigningKeysJson", "()Ljava/lang/String;");
        }

        /// <summary>
        /// Decrypt the given cipher text by performing the necessary signature verification and * decryption (if required) steps based on the protocolVersion
        /// </summary>
        /// <param name="cipherText">cipher text</param>
        /// <returns>plain text</returns>
        public string Decrypt(string cipherText)
        {
            try
            {
                // load configs
                var recipientId = ConfigurationManager.AppSettings["Recipient_Id"];
                if (string.IsNullOrWhiteSpace(recipientId))
                    throw new ConfigurationErrorsException("Setting Recipient_Id must be set.");
                var privateKey = ConfigurationManager.AppSettings["Base64_PKCS8_Private_key"];
                if (string.IsNullOrWhiteSpace(privateKey))
                    throw new ConfigurationErrorsException("Setting Base64_PKCS8_Private_key must be set.");

                // build payment method token recipient
                Class paymentMethodTokenRecipientBuilder = _jniEnv.FindClass("com/google/crypto/tink/apps/paymentmethodtoken/PaymentMethodTokenRecipient$Builder");
                var recipientBuilder = paymentMethodTokenRecipientBuilder.newInstance();
                recipientBuilder.Invoke<Object>("senderVerifyingKeys",
                    "(Ljava/lang/String;)Lcom/google/crypto/tink/apps/paymentmethodtoken/PaymentMethodTokenRecipient$Builder;", _trustedSigningKeysJson);
                recipientBuilder.Invoke<Object>("recipientId", "(Ljava/lang/String;)Lcom/google/crypto/tink/apps/paymentmethodtoken/PaymentMethodTokenRecipient$Builder;",
                   recipientId);
                recipientBuilder.Invoke<Object>("protocolVersion", "(Ljava/lang/String;)Lcom/google/crypto/tink/apps/paymentmethodtoken/PaymentMethodTokenRecipient$Builder;",
                    "ECv2");
                recipientBuilder.Invoke<Object>("addRecipientPrivateKey", "(Ljava/lang/String;)Lcom/google/crypto/tink/apps/paymentmethodtoken/PaymentMethodTokenRecipient$Builder;",
                    privateKey);
                var recipient = recipientBuilder.Invoke<Object>("build", "()Lcom/google/crypto/tink/apps/paymentmethodtoken/PaymentMethodTokenRecipient;");

                // decrypt message
                var plainText = recipient.Invoke<String>("unseal", "(Ljava/lang/String;)Ljava/lang/String;", cipherText);
                return plainText;
            }
            catch (Sys.Exception ex)
            {
                throw ex;
            }

        }
    }
}
