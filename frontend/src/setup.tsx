import { MantineProvider } from "@mantine/core";
import "@mantine/core/styles.css";
import "@mantine/dates/styles.css";
import { QueryClientProvider } from "@tanstack/react-query";
import App from "~/App";
import queryClient from "~/lib/reactQueryClient";

export const createApp = () => (
	<QueryClientProvider client={queryClient}>
		<MantineProvider>
			<App />
		</MantineProvider>
	</QueryClientProvider>
);
