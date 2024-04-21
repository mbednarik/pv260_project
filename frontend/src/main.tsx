import { MantineProvider } from "@mantine/core";
import { QueryClientProvider } from "@tanstack/react-query";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "~/App";
import queryClient from "~/lib/reactQueryClient";

import "@mantine/core/styles.css";
import "@mantine/dates/styles.css";

ReactDOM.createRoot(document.getElementById("root")!).render(
	<React.StrictMode>
		<QueryClientProvider client={queryClient}>
			<MantineProvider>
				<App />
			</MantineProvider>
		</QueryClientProvider>
	</React.StrictMode>
);
