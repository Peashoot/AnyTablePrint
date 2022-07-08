package main

import (
	"log"
	"net/http"
)

func main() {
	http.HandleFunc("/article/setWidget", handle)
	http.ListenAndServe(":6900", nil)
}

func handle(w http.ResponseWriter, r *http.Request) {
	if err := r.ParseMultipartForm(1 << 14); err != nil {
		log.Println("err:", err)
	}
	log.Println(r.MultipartForm)

}
