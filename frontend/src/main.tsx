import React from "react";
import ReactDOM from "react-dom/client";
import { createApp } from "~/setup";

ReactDOM.createRoot(document.getElementById("root")!).render(
	<React.StrictMode>{createApp()}</React.StrictMode>
);
