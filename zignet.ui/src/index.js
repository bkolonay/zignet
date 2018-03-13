import React from 'react';
import ReactDOM from 'react-dom';
import registerServiceWorker from './registerServiceWorker';
import { BrowserRouter } from 'react-router-dom'
// todo: revert below to use regular, not mock
import ZigNetApi from './api/mocks/ZigNetApi'
import App from './App';
import './common/bootstrap.css'

ReactDOM.render((
  	<BrowserRouter>
      <App zigNetApi={new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/')} />
  	</BrowserRouter>
  ), document.getElementById('root')
);
registerServiceWorker();
