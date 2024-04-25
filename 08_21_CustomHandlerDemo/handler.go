package main

import (
	"encoding/json"
	"log"
	"net/http"
	"os"

	"github.com/google/uuid"
)

type InvokeRequest struct {
	Data     map[string]json.RawMessage
	Metadata map[string]interface{}
}

type InvokeResponse struct {
	Outputs     map[string]interface{}
	Logs        []string
	ReturnValue interface{}
}

type Email struct {
	Subject string `json:"subject"`
	Body    string `json:"body"`
}

func messageHandler(w http.ResponseWriter, r *http.Request) {
	var invokeRequest InvokeRequest

	d := json.NewDecoder(r.Body)
	d.Decode(&invokeRequest)

	var reqData map[string]interface{}
	json.Unmarshal(invokeRequest.Data["req"], &reqData)

	var email Email
	json.Unmarshal([]byte(reqData["Body"].(string)), &email)

	message := make(map[string]interface{})
	message["partitionKey"] = ""
	message["rowKey"] = uuid.New()
	message["subject"] = email.Subject
	message["body"] = email.Body

	outputs := make(map[string]interface{})
	outputs["message"] = message

	returnValue := "Message created!"

	invokeResponse := InvokeResponse{outputs, nil, returnValue}

	responseJson, _ := json.Marshal(invokeResponse)

	w.Header().Set("Content-Type", "application/json")
	w.Write(responseJson)
}

func main() {
	listenAddr := ":8080"
	if val, ok := os.LookupEnv("FUNCTIONS_CUSTOMHANDLER_PORT"); ok {
		listenAddr = ":" + val
	}
	http.HandleFunc("/message", messageHandler)
	log.Printf("About to listen on %s. Go to https://127.0.0.1%s/", listenAddr, listenAddr)
	log.Fatal(http.ListenAndServe(listenAddr, nil))
}
