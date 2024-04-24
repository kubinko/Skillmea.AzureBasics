package main

import (
	"fmt"
	"log"
	"net/http"
	"os"
)

func messageHandler(w http.ResponseWriter, r *http.Request) {
	message := "Hello from the Go custom handler. If you provide you name in the query parameter 'name', you will be greeted more personally."
	name := r.URL.Query().Get("name")
	if name != "" {
		message = fmt.Sprintf("Hello, %s. I am Go, nice to meet you.", name)
	}
	fmt.Fprint(w, message)
}

func main() {
	listenAddr := ":8080"
	if val, ok := os.LookupEnv("FUNCTIONS_CUSTOMHANDLER_PORT"); ok {
		listenAddr = ":" + val
	}
	http.HandleFunc("/api/hello", messageHandler)
	log.Printf("About to listen on %s. Go to https://127.0.0.1%s/", listenAddr, listenAddr)
	log.Fatal(http.ListenAndServe(listenAddr, nil))
}
