import React, { Component } from 'react';
import Home from './Home'
import { getSuiteResults } from '../common/api/api.js';
import { getFilter } from '../common/routing/SearchFilter.js'

class HomeRouter extends Component {
  render() {
    return (
      <Home getResults={getSuiteResults} filter={getFilter(window.location.search)} />
    );
  }
}

export default HomeRouter;