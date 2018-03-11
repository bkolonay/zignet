import React from 'react';
import ReactDOM from 'react-dom';
import registerServiceWorker from './registerServiceWorker';

import { BrowserRouter } from 'react-router-dom'

import App from './App';
import ZigNetApi from './api/ZigNetApi'
import './common/bootstrap.css'

ReactDOM.render((
  	<BrowserRouter>
      <App zigNetApi={new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/')} />
  	</BrowserRouter>
  ), document.getElementById('root')
);
registerServiceWorker();
